#version 330 core

in vec3 fragC;
out vec4 FragColor;

void main() {
    FragColor = vec4(fragC, 1.0);
}