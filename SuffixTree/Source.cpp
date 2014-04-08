#include <iostream>
#include <string>
#include <unordered_map>
using namespace std;

class Node;
string str;

class Edge{
public: 
    Edge(int from, int to, Node* n){
        fromTo.first = from;
        fromTo.second = to;
        next = n;
    }
    pair<int, int> fromTo;
    Node* next;
};

typedef unordered_map<string, Edge*> Edges;

class Node{
public:
    Node(){
        suffixLink = nullptr;
    }
    Node* suffixLink;
    Edges edges;
};


int main()
{

    cin >> str;
    Node* root = new Node();
    Node* currentNode = root;
    int remainder = 1;
    Node* activeNode = root;
    int activeLength = 0;
    string activeEdge = "";
    for (size_t i = 0; i < str.length(); i++)
    {
        string currentChar = str.substr(i, 1);
        if (remainder != 1)
        {
            Edges::const_iterator it = activeNode->edges.find(activeEdge);
            if (currentChar == str.substr(it->second->fromTo.first + activeLength, 1))
            {
                if (activeEdge == "")
                    activeEdge = currentChar;
                //find out char again.
                activeLength++;
                remainder++;
                if (it->second->fromTo.second != -1 && (it->second->fromTo.second - it->second->fromTo.first) == activeLength)
                {
                    activeEdge = "";
                    activeLength = 0;
                    activeNode = it->second->next;
                }
            }
            else
            {
                bool isFirstCreated = true;
                Node* preNode = nullptr;
                int number = remainder;
                for (size_t k = 0; k < number; k++)
                {
                    it = activeNode->edges.find(activeEdge);
                    if (it == activeNode->edges.end() && activeLength == 0)
                    {
                        Node* splitNode = activeNode;
                        Node* newNode = new Node();
                        splitNode->edges.insert(make_pair<string, Edge*>(str.substr(i, 1), (new Edge(i, -1, newNode))));
                    }
                    else if (currentChar != str.substr(it->second->fromTo.first + activeLength))
                    {
                        Node* splitNode = it->second->next;
                        Node* firstLeft = new Node();
                        Node* secondLeft = new Node();
                        int splitIndex = it->second->fromTo.first + activeLength;
                        it->second->fromTo.second = splitIndex;
                        splitNode->edges.insert(make_pair<string, Edge*>(str.substr(splitIndex, 1), (new Edge(splitIndex, -1, firstLeft))));
                        splitNode->edges.insert(make_pair<string, Edge*>(str.substr(i, 1), (new Edge(i, -1, secondLeft))));
                        //rule 2
                        if (!isFirstCreated)
                        {
                            preNode->suffixLink = splitNode;
                            preNode = splitNode;
                        }
                        else
                        {
                            isFirstCreated = false;
                            preNode = splitNode;
                        }
                       //rule 1
                        if (activeNode == root)
                        {
                            activeEdge = str.substr(i - k - 1);
                            activeLength--;
                        }
                        else
                        {
                            if (activeNode->suffixLink != nullptr)
                                activeNode = activeNode->suffixLink;
                            else
                                activeNode = root;
                        }
                    }
                }
                remainder = 1;
                activeEdge = "";
                activeLength = 0;
                activeNode = root;
            }
        }
        else if (currentNode->edges.find(currentChar) == currentNode->edges.end())
        {
            //Don't have edge start from current char
            Node* newNode = new Node();
            currentNode->edges.insert(make_pair<string, Edge*>(str.substr(i, 1), (new Edge(i, -1, newNode))));
        }
        else
        {
            activeLength++;
            activeEdge = currentChar;
            remainder++;
        }
    }
    return 0;
}