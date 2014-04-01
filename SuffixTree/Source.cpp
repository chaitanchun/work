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
    string activeEdge = nullptr;
    for (size_t i = 0; i < str.length(); i++)
    {
        string currentChar = str.substr(i, 1);
        Edges::const_iterator it = currentNode->edges.find(currentChar);
        if (it == currentNode->edges.end())
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