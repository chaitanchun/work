#include <iostream>
#include <string>
#include <vector>
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

class Node{
public:
    Node(){
        suffixLink = nullptr;
    }
    Node* suffixLink;
    unordered_map<char, Edge*> edges;


};

int main()
{

    cin >> str;
    Node* root = new Node();
    Node* currentNode = root;
    for (size_t i = 0; i < str.length; i++)
    {
        
        unordered_map<char, Edge*>::const_iterator it = currentNode->edges.find(str.at(i));
        if (it == currentNode->edges.end())
        {
            //Don't have edge start from current char
            Node* newNode = new Node();
            Edge* newEdge = new Edge(i, -1, newNode);

        }
        else
        {

        }



    }
    return 0;
}